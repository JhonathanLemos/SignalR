import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { ClientesComponent } from './clientes/clientes.component';
import { ClienteFormComponent } from './cliente-form/cliente-form.component';
import { ProdutoComponent } from './produto/produto.component';
import { AuthGuard } from './guard/auth.guard';
import { HomeComponent } from './home/home.component';
import { ChatComponent } from './chat/chat.component';
import { CreateServerComponent } from './server/create-server/create-server.component'; 
import { ShowServerComponent } from './server/show-server/show-server.component';
import { NewConversationComponent } from './mensagens/new-conversation/new-conversation.component';
const routes: Routes = [
  {path: '', redirectTo: '/home', pathMatch: 'full'},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: LoginComponent},
  {path: 'clientes', component: ClientesComponent, canActivate: [AuthGuard] },
  {path: 'clientes/:id', component: ClienteFormComponent, canActivate: [AuthGuard] },
  {path: 'clientes/add', component: ClienteFormComponent, canActivate: [AuthGuard] },
  {path: 'produtos', component: ProdutoComponent, canActivate: [AuthGuard] },
  {path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  {path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
  {path: 'create-server', component: CreateServerComponent, canActivate: [AuthGuard] },
  {path: 'show-server/:id', component: ShowServerComponent, canActivate: [AuthGuard] },
  {path: 'new-conversation', component: NewConversationComponent, canActivate: [AuthGuard] },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
