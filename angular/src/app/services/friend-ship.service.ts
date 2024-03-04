import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pagination } from '../entidades/Pagination';
import { Produto } from '../entidades/Produto';
import { FriendShip } from '../entidades/FriendShip';
import { UserAndName } from '../entidades/UserAndName';

@Injectable({
  providedIn: 'root'
})
export class FriendShipService {
  private apiUrl = 'https://localhost:7136/api/friendShip';

  constructor(private http: HttpClient) { }

  getAllFrienshipByUserId(id: string | null): Observable<UserAndName[]> {
    return this.http.get<UserAndName[]>(`${this.apiUrl}/GetAllFrienshipByUserId/${id}`);
  }
}
