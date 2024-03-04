import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsersService } from '../services/users.service';
import { UserAndCode } from '../entidades/UserAndCode';
import { AuthService } from '../services-security/auth.service';
import { SolicitationService } from '../services/solicitation.service';
import { Solicitation } from '../entidades/Solicitation';
import { MatDialog } from '@angular/material/dialog';
import { FriendShipService } from '../services/friend-ship.service';
import { UserAndName } from '../entidades/UserAndName';
import { ConversationService } from '../services/conversation.service';
import { Conversation } from '../entidades/Conversation';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  userId!: string | null;
  show: boolean = true;
  updateNewConversation!: boolean;
  listaSolicitations!: Solicitation[];
  listaAmigos!: UserAndName[];
  attChat!: Boolean;
  constructor(
    private chatService: ChatService,
    private router: Router,
    public dialog: MatDialog,
    private authService: AuthService,
    private solicitationService: SolicitationService,
    private friendShipService: FriendShipService
  ) {}

  async ngOnInit() {
    this.userId = this.authService.getCookie('userId');
    this.getMySolicitations();
    this.getAllFriends();
    await this.chatService.getConncetion();
    await this.startConnection();
  }

  async startConnection() {
    this.userId = this.authService.getCookie('userId');
    this.chatService.hubConnection?.on('newSolicitation', () => {
      this.getMySolicitations();
    });

    this.chatService.hubConnection?.on('attConversation', () => {
      this.getAllFriends();
      this.getMySolicitations();
    });
  }

  atualizarShow() {
    this.attChat = !this.show;
    this.show = true;
  }

  updateConversation() {
    this.show = false;
    this.updateNewConversation = !this.updateNewConversation;
  }

  getMySolicitations() {
    this.solicitationService
      .getSolicitationsByUserId(this.userId)
      .subscribe((res) => {
        this.listaSolicitations = res;
      });
  }

  acceptSolicitaton(user: Solicitation) {
    this.solicitationService.acceptSolicitation(user).subscribe((res) => {
      this.getMySolicitations();
      this.getAllFriends();
    });
  }

  denySolicitaton(user: Solicitation) {
    this.solicitationService.denySolicitation(user).subscribe((res) => {
      this.getMySolicitations();
      this.getAllFriends();
    });
  }

  getAllFriends() {
    this.friendShipService
      .getAllFrienshipByUserId(this.userId)
      .subscribe((res) => {
        this.listaAmigos = res;
        // const dialogRef = this.dialog.open(NewConversationComponent,
        //   {
        //     width: '450px',
        //     height: '350px',
        //     hasBackdrop: false,
        //     data: { lista: listaAmizades }
        //   })
      });
  }
}
