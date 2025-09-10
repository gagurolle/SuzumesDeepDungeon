import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MinecraftContent } from '../../models/minecraft-content';
import { MinecraftMainContent } from '../../models/minecraft-main-content';


@Injectable({
  providedIn: 'root'
})
export class MinecraftService {
  private apiUrl = 'minecraft'; // Базовый URL API

  constructor(private http: HttpClient) { }

  // Получить контент с пагинацией
  getContents(page: number = 1, pageSize: number = 30): Observable<PagedResponse<MinecraftContent>> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PagedResponse<MinecraftContent>>(`${this.apiUrl}/GetContents`, { params });
  } 

  // Создать новый контент
  createContent(content: MinecraftContent): Observable<MinecraftContent> {
    return this.http.post<MinecraftContent>(`${this.apiUrl}/NewContent`, content);
  }

  // Обновить существующий контент
  updateContent(content: MinecraftContent): Observable<MinecraftContent> {
    return this.http.patch<MinecraftContent>(`${this.apiUrl}/UpdateContent`, content);
  }

  // Удалить контент
  deleteContent(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteContent`, { 
      params: { id: id.toString() } 
    });
  }

  // Обновить основной контент
  updateMainContent(content: MinecraftMainContent): Observable<MinecraftMainContent> {
    return this.http.post<MinecraftMainContent>(`${this.apiUrl}/UpdateMainContent`, content);
  }

  // Получить основной контент
  getMainContent(): Observable<MinecraftMainContent> {
    return this.http.get<MinecraftMainContent>(`${this.apiUrl}/GetMainContent`);
  }
}