import { Component, OnInit } from '@angular/core';
import { TokenService } from './services-security/token.service';
import { AuthService } from './services-security/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private tokenService: TokenService, private loginService: AuthService){}
  title = 'auth-angular';

  ngOnInit(){
  }

  isAuthenticated(){
    var a = this.loginService.getCookie("isAuthenticated");
    return a;
   };
   
 
}
