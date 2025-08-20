import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { User } from '../../models/user';
import { StorageService } from '../local-storage/local-storage';



const ClaimTypes = {
  Name: 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name',
  Role: 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
};

@Injectable({ providedIn: 'root' })
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  private apiUrl = 'auth/login';

  public user: User | null = null;

  constructor(private http: HttpClient, private router: Router, private storage: StorageService) {
    this.initializeFromToken();
  }

  ngOnInit(): void {

    this.checkUser()
  }

  checkUser(): User | null {
     if(this.isAuthenticated){
      var p = this.getLocalStorage;
      this.setUserFromToken(p);
    }
    return this.currentUserSubject.value;
  }
  

  login(username: string, password: string): Observable<{ token: string }> {

    return this.http.post<{ token: string }>(this.apiUrl, { username, password }).pipe(
      tap(response => {
        this.storage.setItem('jwt_token', response.token);
        this.setUserFromToken(response.token);
      })
    );
  }

getToken() {
    return this.storage.getItem('jwt_token');
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    this.currentUserSubject.next(null);
    this.router.navigate(['/']);
  }

  private initializeFromToken(): void {
    const token = this.storage.getItem('jwt_token');
    if (token) {
      this.setUserFromToken(token);
    }
  }

  private setUserFromToken(token: string): void {
    try {
      const decoded: any = jwtDecode(token);
      this.currentUserSubject.next({
        username: decoded[ClaimTypes.Name],
        isAdmin: decoded[ClaimTypes.Role] === 'Admin',
      } as User);
      this.user = this.currentUserSubject.value;

    } catch (e) {
      this.logout();
    }
  }

  get isAuthenticated(): boolean {
    return !!this.storage.getItem('jwt_token');
  }

  get getLocalStorage(): string {
    return this.storage.getItem('jwt_token') + '';
  }
  get isAdmin(): boolean {
    return this.currentUserSubject.value?.isAdmin ?? false;
  }
  get getUserName(): string {
    return this.currentUserSubject.value?.username ?? '';
  }
  get getUserRole(): string {
    return this.isAdmin ? 'Admin' : 'User';
  }

  get getUser(): User | null {
    return this.currentUserSubject.value;
  }
}

