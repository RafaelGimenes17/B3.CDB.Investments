import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CDBService, CDBResultadoResponse } from '../services/cdb.service';

@Component({
  selector: 'app-cdb-calculator',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './cdb-calculator.component.html',
  styleUrls: ['./cdb-calculator.component.css']
})
export class CDBCalculatorComponent implements OnInit {
  calculoForm: FormGroup;
  resultado: CDBResultadoResponse | null = null;
  carregando = false;
  erro: string | null = null;
  usarCalculadora2 = false;

  constructor(
    private fb: FormBuilder,
    private cdbService: CDBService
  ) {
    this.calculoForm = this.fb.group({
      valorInicial: ['', [Validators.required, Validators.min(0.01)]],
      meses: ['', [Validators.required, Validators.min(2)]]
    });
  }

  ngOnInit(): void {}

  calcular(): void {
    if (this.calculoForm.invalid) {
      this.erro = 'Por favor, preencha os campos corretamente.';
      return;
    }

    this.carregando = true;
    this.erro = null;
    this.resultado = null;

    const request = this.calculoForm.value;

    const observable = this.usarCalculadora2 
      ? this.cdbService.calcularInvestimento2(request)
      : this.cdbService.calcularInvestimento(request);

    observable.subscribe({
      next: (response) => {
        this.resultado = response;
        this.carregando = false;
      },
      error: (error) => {
        this.erro = 'Erro ao calcular investimento. Verifique os valores informados.';
        console.error('Erro:', error);
        this.carregando = false;
      }
    });
  }

  limpar(): void {
    this.calculoForm.reset();
    this.resultado = null;
    this.erro = null;
  }

  formatarMoeda(valor: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(valor);
  }

  formatarPercentual(valor: number): string {
    return valor.toFixed(2) + '%';
  }
}
