import { Component } from '@angular/core';
import { PaneService } from './pane.service';

@Component({
  selector: 'a-sia-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public paneService: PaneService) {}
  title = 'a-sia';
}
