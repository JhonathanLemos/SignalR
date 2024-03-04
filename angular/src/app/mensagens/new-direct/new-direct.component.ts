import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Conversation } from 'src/app/entidades/Conversation';
import { UserAndCode } from 'src/app/entidades/UserAndCode';
import { UserAndName } from 'src/app/entidades/UserAndName';
import { AuthService } from 'src/app/services-security/auth.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-new-direct',
  templateUrl: './new-direct.component.html',
  styleUrls: ['./new-direct.component.css']
})
export class NewDirectComponent {
  @Input() listaAmigos!: UserAndName[];
  @Input() userId!: string | null;

  constructor(private conversationService: ConversationService, private router: Router){}
  
  createNewConversation(userAndName: UserAndName){
    let conversation = new Conversation();
    conversation.firstUserId = this.userId;
    conversation.secondUserId = userAndName.userId;
    this.conversationService.createNewConversation(conversation).subscribe(res => {
    })
  }
}
