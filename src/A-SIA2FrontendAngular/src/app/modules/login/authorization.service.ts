import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  constructor(private http: HttpClient) {}
  public loggedIn = false;

  login(username: string, password: string): Observable<HttpResponse<unknown>> {
    return this.http.post<unknown>(
      environment.aSiaServer + '/User/Login',
      { username: username, password: password },
      { observe: 'response' }
    );
  }
}
