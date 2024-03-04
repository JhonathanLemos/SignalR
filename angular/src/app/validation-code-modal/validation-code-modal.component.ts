import { Component, Inject, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EmailCodeService } from '../services/email-code.service';
import { EmailCode } from '../entidades/EmailCode';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AlertComponent } from '../alert/alert.component';
import { Router } from '@angular/router';
import { AuthService } from '../services-security/auth.service';

@Component({
  selector: 'app-validation-code-modal',
  templateUrl: './validation-code-modal.component.html',
  styleUrls: ['./validation-code-modal.component.css']
})
export class ValidationCodeModalComponent {
  @ViewChild(AlertComponent) alert !: AlertComponent;

  formulario !: FormGroup;
  userId: any;
  forms: any;
  constructor(private route: Router, private form: FormBuilder, private service: EmailCodeService, @Inject(MAT_DIALOG_DATA) public data: any, private dialogRef: MatDialogRef<ValidationCodeModalComponent>, private authService: AuthService) {
    this.formulario = this.form.group({
      userId: new FormControl(''),
      code: new FormControl('', Validators.required),
    })

    this.userId = this.data.userId;
    this.forms = this.data.form;
  }

  verifyCode() {
    this.formulario.get('userId')?.setValue(this.userId);
    this.service.verifyCode(this.formulario.value).subscribe({
      next: (res) => {
        this.dialogRef.close("ok");
        this.authService.login(this.forms).subscribe({
          next: (res) => {
            debugger
            this.route.navigate(['/home'])

          }, error: (error) => {
            this.alert.showCustomAlert("Usuário ou senha inválidos", 'error')
          }
        })
        this.route.navigate(['/login'])
      }, error: (error) => {
        this.alert.showCustomAlert("Insira um código válido", 'error')
      }
    })
  }
}
