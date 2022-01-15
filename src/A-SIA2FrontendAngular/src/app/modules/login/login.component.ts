import { Component } from '@angular/core';
import { AuthorizationService } from './authorization.service';

@Component({
  selector: 'a-sia-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  constructor(private authorizationService: AuthorizationService) {}

  username = '';
  password = '';

  onLogin = (): void => {
    this.authorizationService.login(this.username, this.password).subscribe((data) => {
      console.log(data.status);
      if (data.status == 200) {
        this.authorizationService.loggedIn = true;
        console.log('logged in');
      } else console.log('login fail');
    });
  };

  onCancel = (): void => {
    this.username = '';
    this.password = '';
  };
}
