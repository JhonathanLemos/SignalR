import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmailCode } from '../entidades/EmailCode';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmailCodeService {
  private apiUrl = 'https://localhost:7136/api/EmailCode';
  constructor(private http: HttpClient) { }

  verifyCode(emailCode: EmailCode): Observable<EmailCode> {
    return this.http.post<EmailCode>(`${this.apiUrl}/VerifyCode`, emailCode);
  }
}
