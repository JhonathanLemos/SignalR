import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, firstValueFrom, map, max } from 'rxjs';
import { Conversation } from '../entidades/Conversation';
import { UserAndName } from '../entidades/UserAndName';
import { UserAndNameAndLastMessage } from '../entidades/UserAndNameAndLastMessage';
import { Message } from '../entidades/Message';
import { PaginationMessages } from '../entidades/PaginationMessages';
@Injectable({
  providedIn: 'root'
})
export class ConversationService {
  private apiUrl = 'https://localhost:7136/api/Conversation';
  constructor(private http: HttpClient) { }


  createNewConversation(conversation: Conversation): Observable<Conversation> {
    return this.http.post<Conversation>(`${this.apiUrl}`, conversation);
  }

  getAllConversationsByUserId(userId: string | null): Observable<UserAndNameAndLastMessage[]> {
    return this.http.get<UserAndNameAndLastMessage[]>(`${this.apiUrl}/GetAllConversationsByUserId/${userId}`,);
  }

  async getAllMessagesFromConversationByUserId(conversation: Conversation, paginationMessages: PaginationMessages): Promise<Message[]> {
    try {
      const response = await firstValueFrom(this.http.get<Message[]>(`${this.apiUrl}/getAllMessagesFromConversationByUserId/${conversation.firstUserId}/${conversation.secondUserId}/${paginationMessages?.pagina}/${paginationMessages?.tamanhoPagina}`));
      return response;
    } catch (error) {
      throw error;
    }
  }
  
  
}