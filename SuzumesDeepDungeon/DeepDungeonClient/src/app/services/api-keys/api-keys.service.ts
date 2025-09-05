import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ExternalApi } from '../../models/external-api';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiKeysService {


  apiUrl = 'ext';

  constructor(private http: HttpClient, private router: Router,) {
  }


  getApiKeys(username: string): Observable<ExternalApi[]> {
    let httpParams = new HttpParams();

    httpParams = httpParams.append('username', username);

    return this.http.get<ExternalApi[]>(this.apiUrl, { params: httpParams });
  }

  deleteApiKey(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  addNewApiKey(api_entity: ExternalApi): Observable<ExternalApi> {
    return this.http.post<ExternalApi>(`${this.apiUrl}`, api_entity).pipe(
      catchError(error => {
        console.error('Error adding game:', error);
        return throwError(() => error);
      })
    );


  }
}
