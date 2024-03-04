import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, map, throwError } from 'rxjs';
import { Token } from '../entidades/token';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7136/api/login'; // Substitua pela URL da sua API .NET

  constructor(private http: HttpClient, private route: Router) {
  }



  login(login: []): Observable<Token> {
    return this.http.post<Token>(this.apiUrl, login).pipe(
      map((res) => {
        if (res.value == "EmailNotValidated")
          return res;
          this.setCookie('token', res.value.token, 1); // Define o cookie para expirar em 1 dia
          this.setCookie('isAuthenticated', 'true', 1); 
          this.setCookie('userName', res.value.userName, 1); 
          this.setCookie('userId', res.value.userId, 1); 
          this.setCookie('userCode', res.value.userCode, 1); 
        return res;
      }),
      catchError((error: any) => {
        console.error('Ocorreu um erro durante o login:', error);
        return throwError(() => error); 
      })
    );
  }

  logout() {
    this.deleteCookie('token');
    this.deleteCookie('isAuthenticated');
    this.deleteCookie('userId');
    this.route.navigate(['/login'])
    return this.http.post(`${this.apiUrl}/logout`, '')
  }

  register(login: []) {
    return this.http.post(`${this.apiUrl}/register`, login)
  }

  generateCode(login: []) {
    return this.http.post(`${this.apiUrl}/GenCode`, login)
  }
  setCookie(name: string, value: string, days: number) {
    const date = new Date();
    date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
  
  
    // Constrói o cookie com todas as opções
    document.cookie = name + "=" + value + ";";
  }

getCookie(name: string): string | null {
  const cookieName = name + "=";
  const decodedCookie = decodeURIComponent(document.cookie);
  const cookieArray = decodedCookie.split(';');
  for (let i = 0; i < cookieArray.length; i++) {
    let cookie = cookieArray[i];
    while (cookie.charAt(0) == ' ') {
      cookie = cookie.substring(1);
    }
    if (cookie.indexOf(cookieName) == 0) {
      return cookie.substring(cookieName.length, cookie.length);
    }
  }
  return null;
}
  private deleteCookie(name: string) {
    document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
  }
}
