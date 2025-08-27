import { Component } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';

@Component({
  selector: 'app-minecraft-server',
  standalone: true,
  imports: [MatCardModule, MatButtonModule],
  templateUrl: './minecraft-server.component.html',
  styleUrl: './minecraft-server.component.css'
})
export class MinecraftServerComponent {

}
