import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FindGameDTO } from './models/game-find';
import { catchError, Observable, throwError } from 'rxjs';
import { GameRankDTO } from './models/game-ranking';

@Injectable({
  providedIn: 'root'
})
export class GameFindServiceService {
  private apiUrl = 'api/gameService/'; // Замените на ваш URL
  constructor(private http: HttpClient) {}

  // Find Game in RAWG
  getGames(gameName: string): Observable<FindGameDTO[]> {
    let params = new HttpParams()
      .set('gameName', gameName)
    return this.http.get<FindGameDTO[]>(`${this.apiUrl}findGameData`, { params });
  }

  getGameData(game: FindGameDTO): Observable<GameRankDTO> {
      return this.http.post<GameRankDTO>(`${this.apiUrl}getGameData`, game).pipe(
    catchError(error => {
      console.error('Error adding game:', error);
      return throwError(() => error);
    })
  );
  }
}
