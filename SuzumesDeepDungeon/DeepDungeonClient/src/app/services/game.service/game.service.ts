import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { GameRankDTO } from '../../models/game-ranking';



@Injectable({
  providedIn: 'root'
})
export class GameService {
  private apiUrl = 'DeepDungeon/';

  constructor(private http: HttpClient) {}

  getGames(params?: any): Observable<GameRankDTO[]> {

    let httpParams = new HttpParams();
    
    if (params) {
      Object.keys(params).forEach(key => {
        if (params[key] !== null && params[key] !== undefined) {
          httpParams = httpParams.append(key, params[key].toString());
        }
      });
    }

    return this.http.get<GameRankDTO[]>(this.apiUrl, { params: httpParams });
  }

  updateGame(game: GameRankDTO): Observable<GameRankDTO> {
    return this.http.patch<GameRankDTO>(`${this.apiUrl}`, game);
  }

  deleteGame(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}${id}`);
  }
  addGame(game: GameRankDTO): Observable<GameRankDTO> {
  return this.http.post<GameRankDTO>(`${this.apiUrl}`, game).pipe(
    catchError(error => {
      console.error('Error adding game:', error);
      return throwError(() => error);
    })
  );
}
}
