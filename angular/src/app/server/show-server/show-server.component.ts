import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Channel } from 'src/app/entidades/Channel';
import { Server } from 'src/app/entidades/Server';
import { ServerService } from 'src/app/services/server.service';

@Component({
  selector: 'app-show-server',
  templateUrl: './show-server.component.html',
  styleUrls: ['./show-server.component.css'],
})
export class ShowServerComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private serverService: ServerService
  ) {}
  list!: Server[];
  serverId!: number;
  ngOnInit() {
    this.serverId = this.route.snapshot.params['id'];
    this.getServer();
  }

  getServer() {
    this.serverService.getServerById(this.serverId).subscribe((x) => {
      this.list = x;
    });
  }

  openChannel(channel: Channel){
  }
}
