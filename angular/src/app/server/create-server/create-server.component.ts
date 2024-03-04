import { Component, Inject } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { ServerBarComponent } from "src/app/components/server/server-bar.component";
import { Server } from "src/app/entidades/Server";
import { AuthService } from "src/app/services-security/auth.service";
import { ServerService } from "src/app/services/server.service";


@Component({
  selector: 'app-create-server',
  templateUrl: './create-server.component.html',
  styleUrls: ['./create-server.component.css']
})
export class CreateServerComponent {
  serverForm!: FormGroup;
  adminId!: string | null;
  constructor(private formBuilder: FormBuilder, private serverService: ServerService, @Inject(MAT_DIALOG_DATA) public data: any, private dialogRef: MatDialogRef<ServerBarComponent>) { }

  ngOnInit(): void {
    this.adminId = this.data.adminId;
    this.serverForm = this.formBuilder.group({
      serverName: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.serverForm.valid) {
      let server = new Server();
      server.serverName = this.serverForm.value.serverName;
      server.adminId = this.adminId;
      this.serverService.create(server).subscribe(res => {
        this.dialogRef.close()
      })
    }
  }
}
