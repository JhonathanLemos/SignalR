import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {MatTableModule} from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { ConfirmationAlertComponent } from './confirmation-alert/confirmation-alert.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AuthInterceptor } from './guard/AuthInterceptor';
import { HeaderComponent } from './header/header.component';
import { ClienteFormComponent } from './cliente-form/cliente-form.component';
import { ClientesComponent } from './clientes/clientes.component';
import { LoginComponent } from './login/login.component';
import { FormularioComponent } from './modais/formulario/formulario.component';
import { ProdutoComponent } from './produto/produto.component';
import { AuthGuard } from './guard/auth.guard';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AlertComponent } from './alert/alert.component';
import {MatCardModule} from '@angular/material/card';
import { ValidationCodeModalComponent } from './validation-code-modal/validation-code-modal.component';
import { HomeComponent } from './home/home.component';
import { TopoHomeComponent } from './topo-home/topo-home.component';
import { ChatComponent } from './chat/chat.component';
import { MessageInputComponent } from './message-input/message-input.component';
import { ConversationsComponent } from './conversations/conversations.component';
import { ServerBarComponent } from './components/server/server-bar.component';
import { CreateServerComponent } from './server/create-server/create-server.component';
import { ShowServerComponent } from './server/show-server/show-server.component';
import { MatFormFieldControl } from '@angular/material/form-field';
import { DirectComponent } from './mensagens/direct/direct.component';
import { NewDirectComponent } from './mensagens/new-direct/new-direct.component';
import { SendSolicitationComponent } from './mensagens/send-solicitation/send-solicitation.component';
import { NewConversationComponent } from './mensagens/new-conversation/new-conversation.component';
import { FirstLetterPipe } from './util/first-letter.pipe';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    ConfirmationAlertComponent,
    ClientesComponent,
    ClienteFormComponent,
    LoginComponent,
    FormularioComponent,
    ProdutoComponent,
    AlertComponent,
    ValidationCodeModalComponent,
    HomeComponent,
    TopoHomeComponent,
    ChatComponent,
    MessageInputComponent,
    ConversationsComponent,
    ServerBarComponent,
    CreateServerComponent,
    ShowServerComponent,
    DirectComponent,
    NewDirectComponent,
    SendSolicitationComponent,
    NewConversationComponent,
    FirstLetterPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatButtonModule,
    MatToolbarModule,
    BrowserAnimationsModule,
    MatIconModule,
    MatDialogModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatTableModule,
    FormsModule,
    MatPaginatorModule,
    MatSidenavModule,
    MatCardModule,
  ],
  providers: [{ provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true}, AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
