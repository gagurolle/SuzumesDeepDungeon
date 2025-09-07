import { Injectable } from '@angular/core';
import { TwitchAction } from '../models/twitch';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class GamesFromFollowersTwitchService {
  private apiUrl = 'twitch/getGameList';
  constructor(private http: HttpClient) { }

  getGames(): Observable<TwitchAction[]> {
    // let params = new HttpParams()
    //   .set('gameName', gameName)
    return this.http.get<TwitchAction[]>(`${this.apiUrl}`);
  }
}

