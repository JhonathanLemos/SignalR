import { Component, OnInit } from '@angular/core';
import { ServerDto } from 'src/app/entidades/ServerDto';
import { ServerService } from 'src/app/services/server.service';
import { UsersService } from 'src/app/services/users.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateServerComponent } from 'src/app/server/create-server/create-server.component'; 
import { AuthService } from 'src/app/services-security/auth.service';
@Component({
  selector: 'app-server',
  templateUrl: './server-bar.component.html',
  styleUrls: ['./server-bar.component.css']
})
export class ServerBarComponent implements OnInit{
  listaServidores: ServerDto[] = [];
  userId!: string | null;

  constructor(private authService: AuthService,private userService: UsersService, private serverService: ServerService, private router: Router, public dialog: MatDialog) {
  }

  ngOnInit() {
    this.userId = this.authService.getCookie("userId");
    this.getServers();

  }

  getServers() {
    this.userService.getServersByUserId(this.userId).subscribe(res => {
      this.listaServidores = res
    });
  }

  redirectToCreateServer(){
    const dialogRef = this.dialog.open(CreateServerComponent,
      {
        width: '600px',
        hasBackdrop: false,
        data: { mensagem: 'Cadastrar produto', adminId: this.userId },
      })

      dialogRef.afterClosed().subscribe(result => {
      })
  }

  openServer(number: Number){
   this.router.navigate([`/show-server/${number}`])
  }

  redirectToHome(){
   this.router.navigate(['/home'])
    
  }
}
