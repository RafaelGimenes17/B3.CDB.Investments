import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CalculoCDBRequest {
  valorInicial: number;
  meses: number;
}

export interface ResultadoDetalhado {
  valor: number;
  rendimento: number;
}

export interface CDBResultadoResponse {
  valorInicial: number;
  meses: number;
  aliquota: number;
  resultadoBruto: ResultadoDetalhado;
  resultadoLiquido: ResultadoDetalhado;
  imposto: number;
}

@Injectable({
  providedIn: 'root'
})
export class CDBService {
  private apiUrl = '/api/cdb'; // Usa proxy em desenvolvimento para http://localhost:7257

  constructor(private http: HttpClient) { }

  calcularInvestimento(request: CalculoCDBRequest): Observable<CDBResultadoResponse> {
    return this.http.post<CDBResultadoResponse>(`${this.apiUrl}/calcular`, request);
  }

  calcularInvestimento2(request: CalculoCDBRequest): Observable<CDBResultadoResponse> {
    return this.http.post<CDBResultadoResponse>(`${this.apiUrl}/calcular2`, request);
  }
}
