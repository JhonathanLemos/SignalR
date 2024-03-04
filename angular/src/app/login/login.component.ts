import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../services-security/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertComponent } from '../alert/alert.component';
import { MatDialog } from '@angular/material/dialog';
import { ValidationCodeModalComponent } from '../validation-code-modal/validation-code-modal.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  @ViewChild(AlertComponent) alert !: AlertComponent;

  formulario!: FormGroup;
  routeName !: string;
  mensagem!: string;
  input_visibility: Boolean = true;
  input_visibilityConfirm: Boolean = true;
  constructor(private service: AuthService, private route: Router, public dialog: MatDialog) {
    this.formulario = new FormGroup({
      id: new FormControl(0, Validators.required),
      userName: new FormControl(''),
      email: new FormControl(''),
      password: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]).{8,}$/)]),
      confirmPassword: new FormControl('', ) // Novo campo para repetir a senha
    });

    this.formulario.valueChanges.subscribe(() => {
      this.onFormValuesChanged();
    });
  }

  ngOnInit(): void {
    this.checkRoute();
    this.validateEmail();
  }

  onFormValuesChanged(): void {
    this.passwordsMatchValidator(this.formulario);
  }

  passwordsMatchValidator(formGroup: FormGroup): void {
    var teste = this.validateRoute()
    debugger
    if (this.validateRoute())
    {
      const confirmPassword = this.formulario.get('confirmPassword');
      confirmPassword?.clearValidators();

    }
    else{

      
      const passwordControl = formGroup.get('password');
      const confirmPasswordControl = formGroup.get('confirmPassword');
      
      if (passwordControl && confirmPasswordControl && passwordControl.value !== confirmPasswordControl.value) {
        confirmPasswordControl.setErrors({ 'passwordMismatch': true });
      } else {
        confirmPasswordControl?.setErrors(null);
      }
    }
  }

  // Outros métodos do componente

  validateEmail() {
    const emailControl = this.formulario.get('email');
    const userNameControl = this.formulario.get('userName');

    if (!this.validateRoute()) {
      emailControl?.setValidators([Validators.required, Validators.pattern(/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/)])
      userNameControl?.setValidators([Validators.required, Validators.minLength(4)]);
    }
    else {
      emailControl?.clearValidators();
      userNameControl?.clearValidators();
    }
  }

  validateRoute() {
    return this.routeName == "login" ? true : false
  }
  checkRoute() {
    if (this.route.url == "/login")
      this.routeName = 'login'
    else
      this.routeName = "register"
  }

  register() {
    let login = this.formulario.value
    this.service.register(login).subscribe({
      next: (res: any) => {
        this.alert.showCustomAlert("Cadastro realizado com sucesso!", 'success');
        this.dialog.open(ValidationCodeModalComponent, {
          width: '600px',
          hasBackdrop: false,
          data: { userId: res.userId, form: this.formulario.value }
        })
      }, error: (error) => {
        this.alert.showCustomAlert(error.error[0].description, 'error');
      }
    }

    );

  }

  login() {
    let login = this.formulario.value;
    this.service.login(login).subscribe({
      next: (res) => {
        if (res.value == "EmailNotValidated")
          this.service.generateCode(this.formulario.value).subscribe( {next: (res) => {
          }, error: (error) => {
            if (error.error.registrationResult = "ValidateUser")
            this.dialog.open(ValidationCodeModalComponent, {
              width: '600px',
              hasBackdrop: false,
              data: { userId: error.error.userId }
            })
          }});
        else
          this.route.navigate(['/home']);
      },
      error : (error) => {
        this.alert.showCustomAlert("Login ou senha inválidos", 'error');
      }

    });
  }

  navigateToRegister() {
    if (this.validateRoute()) {
      this.route.navigate(['/register']);
    }
  }

  changeInput() {
    this.input_visibility = !this.input_visibility;
  }

  changeInputConfirm() {
    this.input_visibilityConfirm = !this.input_visibilityConfirm;
  }
}
