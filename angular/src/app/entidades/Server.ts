import { Channel } from "./Channel";
import { UserServerDto } from "./UserServerDto";

export class Server {
    id!: Number;
    serverName!: string;
    adminId!: string | null;
    channels!: Channel[];
    userServers!: UserServerDto;
}