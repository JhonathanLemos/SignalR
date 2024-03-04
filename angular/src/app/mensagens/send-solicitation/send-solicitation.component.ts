import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserAndCode } from 'src/app/entidades/UserAndCode';
import { AuthService } from 'src/app/services-security/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-send-solicitation',
  templateUrl: './send-solicitation.component.html',
  styleUrls: ['./send-solicitation.component.css']
})
export class SendSolicitationComponent {
  formulario!: FormGroup;
  userId!: string | null;

  constructor(private chatService: ChatService, private form: FormBuilder, private userService: UsersService, private authService: AuthService, private router: Router) {
    this.formulario = this.form.group({
      codigoAmizade: ['', Validators.required],
    })
  }

  async ngOnInit() {
    this.userId = this.authService.getCookie("userId");
  }

  async sendSolicitation() {
    let codigo = this.formulario.value.codigoAmizade;
    let userAndCode = new UserAndCode();
    userAndCode.UserCode = codigo;
    userAndCode.userId = this.userId;
    // await this.chatService.sendSolicitation(userAndCode)
    this.userService.sendSolicitation(userAndCode).subscribe(res => {
    })
  }

}
