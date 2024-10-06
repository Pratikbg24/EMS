import { Component } from '@angular/core';
import { TokenStorageService } from '../service/token-storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  role:any
  isLoggedIn = false;
  showAdminBoard = false;
  showModeratorBoard = false;
  username?: string;

  constructor(private tokenStorageService: TokenStorageService) { }

  ngOnInit(): void {
    this.isLoggedIn = !!this.tokenStorageService.getToken();

    if (this.isLoggedIn) {
      debugger
      const user = this.tokenStorageService.getUser();
      this.role = user.role;
 
      this.username = user.firstName;
    }
  } 
  logout(): void {
    this.tokenStorageService.signOut();
    window.location.reload();
  }
}
