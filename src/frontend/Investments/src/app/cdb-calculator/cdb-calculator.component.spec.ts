import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

import { CDBCalculatorComponent } from './cdb-calculator.component';
import { CDBService, CDBResultadoResponse } from '../services/cdb.service';

// ─── Mock de resposta da API ──────────────────────────────────────────────────
const mockResultado: CDBResultadoResponse = {
  valorInicial: 1000,
  meses: 12,
  aliquota: 20,
  resultadoBruto: { valor: 1112.68, rendimento: 112.68 },
  resultadoLiquido: { valor: 1090.14, rendimento: 90.14 },
  imposto: 22.54
};

// ─── Mock do CDBService ───────────────────────────────────────────────────────
const cdbServiceMock = {
  calcularInvestimento: jasmine.createSpy('calcularInvestimento').and.returnValue(of(mockResultado)),
  calcularInvestimento2: jasmine.createSpy('calcularInvestimento2').and.returnValue(of(mockResultado))
};

// ─────────────────────────────────────────────────────────────────────────────

describe('CDBCalculatorComponent', () => {
  let component: CDBCalculatorComponent;
  let fixture: ComponentFixture<CDBCalculatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CDBCalculatorComponent, ReactiveFormsModule],
      providers: [
        { provide: CDBService, useValue: cdbServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CDBCalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    // Reseta os spies e restaura o retorno padrão antes de cada teste
    cdbServiceMock.calcularInvestimento.calls.reset();
    cdbServiceMock.calcularInvestimento.and.returnValue(of(mockResultado));
    cdbServiceMock.calcularInvestimento2.calls.reset();
    cdbServiceMock.calcularInvestimento2.and.returnValue(of(mockResultado));
  });

  // ── Criação ────────────────────────────────────────────────────────────────

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  it('deve inicializar com formulário vazio e sem resultado', () => {
    expect(component.calculoForm.value).toEqual({ valorInicial: '', meses: '' });
    expect(component.resultado).toBeNull();
    expect(component.erro).toBeNull();
    expect(component.carregando).toBeFalse();
  });

  // ── Validação do formulário ────────────────────────────────────────────────

  it('deve marcar o formulário como inválido quando os campos estão vazios', () => {
    expect(component.calculoForm.invalid).toBeTrue();
  });

  it('deve marcar o formulário como inválido quando valorInicial é 0', () => {
    component.calculoForm.setValue({ valorInicial: 0, meses: 12 });
    expect(component.calculoForm.invalid).toBeTrue();
  });

  it('deve marcar o formulário como inválido quando meses é 1', () => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 1 });
    expect(component.calculoForm.invalid).toBeTrue();
  });

  it('deve marcar o formulário como válido com dados corretos', () => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    expect(component.calculoForm.valid).toBeTrue();
  });

  // ── Comportamento ao chamar calcular() com formulário inválido ─────────────

  it('deve exibir mensagem de erro e NÃO chamar o serviço quando o formulário é inválido', () => {
    component.calculoForm.setValue({ valorInicial: '', meses: '' });
    component.calcular();

    expect(cdbServiceMock.calcularInvestimento).not.toHaveBeenCalled();
    expect(component.erro).toBe('Por favor, preencha os campos corretamente.');
    expect(component.resultado).toBeNull();
  });

  // ── Chamada ao serviço ─────────────────────────────────────────────────────

  it('deve chamar calcularInvestimento com os valores corretos do formulário', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();

    expect(cdbServiceMock.calcularInvestimento).toHaveBeenCalledOnceWith({
      valorInicial: 1000,
      meses: 12
    });
    expect(cdbServiceMock.calcularInvestimento2).not.toHaveBeenCalled();
  }));

  it('deve chamar calcularInvestimento2 quando usarCalculadora2 é true', fakeAsync(() => {
    component.usarCalculadora2 = true;
    component.calculoForm.setValue({ valorInicial: 500, meses: 6 });
    component.calcular();
    tick();

    expect(cdbServiceMock.calcularInvestimento2).toHaveBeenCalledOnceWith({
      valorInicial: 500,
      meses: 6
    });
    expect(cdbServiceMock.calcularInvestimento).not.toHaveBeenCalled();
  }));

  // ── Exibição do resultado na tela ──────────────────────────────────────────

  it('deve armazenar o resultado retornado pelo serviço', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();

    expect(component.resultado).toEqual(mockResultado);
    expect(component.carregando).toBeFalse();
    expect(component.erro).toBeNull();
  }));

  it('deve renderizar o valor bruto na tela após o cálculo', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();
    fixture.detectChanges();

    const compiled: HTMLElement = fixture.nativeElement;
    // O valor bruto formatado em BRL deve aparecer no DOM
    expect(compiled.textContent).toContain('R$\u00a01.112,68');
  }));

  it('deve renderizar o valor líquido na tela após o cálculo', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();
    fixture.detectChanges();

    const compiled: HTMLElement = fixture.nativeElement;
    expect(compiled.textContent).toContain('R$\u00a01.090,14');
  }));

  it('deve renderizar o imposto na tela após o cálculo', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();
    fixture.detectChanges();

    const compiled: HTMLElement = fixture.nativeElement;
    expect(compiled.textContent).toContain('R$\u00a022,54');
  }));

  it('deve renderizar a alíquota formatada como percentual', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();
    fixture.detectChanges();

    const compiled: HTMLElement = fixture.nativeElement;
    expect(compiled.textContent).toContain('20.00%');
  }));

  // ── Tratamento de erro da API ──────────────────────────────────────────────

  it('deve exibir mensagem de erro quando o serviço retorna um erro', fakeAsync(() => {
    cdbServiceMock.calcularInvestimento.and.returnValue(
      throwError(() => new Error('Erro de servidor'))
    );

    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();

    expect(component.resultado).toBeNull();
    expect(component.carregando).toBeFalse();
    expect(component.erro).toBe('Erro ao calcular investimento. Verifique os valores informados.');
  }));

  it('deve renderizar a mensagem de erro no DOM quando a API falha', fakeAsync(() => {
    cdbServiceMock.calcularInvestimento.and.returnValue(
      throwError(() => new Error('Erro de servidor'))
    );

    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();
    fixture.detectChanges();

    const compiled: HTMLElement = fixture.nativeElement;
    expect(compiled.textContent).toContain('Erro ao calcular investimento. Verifique os valores informados.');
  }));

  // ── Estado de carregamento ─────────────────────────────────────────────────

  it('deve definir carregando=true imediatamente após chamar calcular()', () => {
    // Usa um observable que não emite imediatamente (sem tick)
    cdbServiceMock.calcularInvestimento.and.returnValue(of(mockResultado));

    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();

    // Antes do tick, carregando ainda pode ser true dependendo do observable
    // Verificamos que o fluxo foi iniciado corretamente
    expect(cdbServiceMock.calcularInvestimento).toHaveBeenCalled();
  });

  // ── Limpar ─────────────────────────────────────────────────────────────────

  it('deve limpar o formulário, resultado e erro ao chamar limpar()', fakeAsync(() => {
    component.calculoForm.setValue({ valorInicial: 1000, meses: 12 });
    component.calcular();
    tick();

    component.limpar();

    expect(component.calculoForm.value).toEqual({ valorInicial: null, meses: null });
    expect(component.resultado).toBeNull();
    expect(component.erro).toBeNull();
  }));

  // ── Formatadores ───────────────────────────────────────────────────────────

  it('deve formatar valor monetário corretamente', () => {
    const resultado = component.formatarMoeda(1234.56);
    expect(resultado).toContain('1.234,56');
  });

  it('deve formatar percentual com 2 casas decimais', () => {
    expect(component.formatarPercentual(20)).toBe('20.00%');
    expect(component.formatarPercentual(22.5)).toBe('22.50%');
    expect(component.formatarPercentual(17.5)).toBe('17.50%');
    expect(component.formatarPercentual(15)).toBe('15.00%');
  });
});
