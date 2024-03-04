import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pagination } from '../entidades/Pagination';
import { Server } from '../entidades/Server';
import { Channel } from '../entidades/Channel';

@Injectable({
  providedIn: 'root'
})
export class ServerService {

  private apiUrl = 'https://localhost:7136/api/server';

  constructor(private http: HttpClient) { }

  // Listar todos os produtos
  getServersByUserId(userId: number): Observable<Server> {
    return this.http.get<Server>(`${this.apiUrl}/${userId}`);
  }

  create(server: Server): Observable<Server> {
    return this.http.post<Server>(`${this.apiUrl}`, server);
  }

  getServerById(userId: number): Observable<Server[]> {
    return this.http.get<Server[]>(`${this.apiUrl}/${userId}`);
  }
}
