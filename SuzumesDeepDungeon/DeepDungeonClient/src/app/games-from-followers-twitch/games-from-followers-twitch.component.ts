import { ChangeDetectorRef, Component } from '@angular/core';
import { GamesFromFollowersTwitchService } from '../services/games-from-followers-twitch.service';
import { TwitchAction } from '../models/twitch';

@Component({
  selector: 'app-games-from-followers-twitch',
  standalone: true,
  imports: [],
  templateUrl: './games-from-followers-twitch.component.html',
  styleUrl: './games-from-followers-twitch.component.css'
})
export class GamesFromFollowersTwitchComponent {

  public actions: TwitchAction[] = [];

  constructor(public service: GamesFromFollowersTwitchService, private cdr: ChangeDetectorRef,){

  }
ngOnInit(){
this.loadActions();
}

loadActions(){

  this.service.getGames().subscribe(x => {
    this.actions = x;
    this.cdr.detectChanges();
  })
}

}
