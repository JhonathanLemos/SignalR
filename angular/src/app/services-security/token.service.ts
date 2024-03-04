import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private apiUrl = 'https://localhost:7136/api/tokenvalidation'; // Substitua pela URL da sua API .NET
  constructor(private http: HttpClient) { }

  isAuthenticated() {
    const token = this.getToken('token');
    var data = {token: token}
    var result =  this.http.post(`${this.apiUrl}`, data)
    return result
  }

  getToken(name: string){
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
    return '';
  }
}
