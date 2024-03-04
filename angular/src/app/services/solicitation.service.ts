import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pagination } from '../entidades/Pagination';
import { Produto } from '../entidades/Produto';
import { Solicitation } from '../entidades/Solicitation';

@Injectable({
  providedIn: 'root'
})
export class SolicitationService {
  private apiUrl = 'https://localhost:7136/api/solicitation';
  constructor(private http: HttpClient) { }

  getSolicitationsByUserId(id: string | null): Observable<Solicitation[]> {
    return this.http.get<Solicitation[]>(`${this.apiUrl}/GetSolicitationsByUserId/${id}`);
  }

  acceptSolicitation(solicitation: Solicitation): Observable<Solicitation> {
    return this.http.post<Solicitation>(`${this.apiUrl}/AcceptSolicitation`, solicitation);
  }

  denySolicitation(solicitation: Solicitation): Observable<Solicitation> {
    return this.http.post<Solicitation>(`${this.apiUrl}/DenySolicitation`, solicitation);
  }

}
