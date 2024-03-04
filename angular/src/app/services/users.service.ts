import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Pagination } from '../entidades/Pagination';
import { Server } from '../entidades/Server';
import { UserAndCode } from '../entidades/UserAndCode';
import { User } from '../entidades/User';
import { UserName } from '../entidades/Username';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private apiUrl = 'https://localhost:7136/api/user';

  constructor(private http: HttpClient) { }

  getServersByUserId(userId: string | null): Observable<Server[]> {
    return this.http.get<Server[]>(`${this.apiUrl}/GetServersByUserId/${userId}`);
  }

  sendSolicitation(userAndCode: UserAndCode): Observable<UserAndCode> {
    return this.http.post<UserAndCode>(`${this.apiUrl}/SendSolicitation`, userAndCode);
  }


  getFriendsById(userId: string | null): Observable<Server[]> {
    return this.http.get<Server[]>(`${this.apiUrl}/GetFriendsById/${userId}`);
  }

  getUserNameById(userId: string | null): Observable<UserName> {
    return this.http.get<UserName>(`${this.apiUrl}/GetUserNameById/${userId}`);
  }
}
