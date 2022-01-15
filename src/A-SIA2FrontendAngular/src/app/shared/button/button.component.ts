import { Component, HostListener, Input } from '@angular/core';

@Component({
  selector: 'a-sia-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @HostListener('click') onClick(): void {
    this.action();
  }
  @Input() action = (): void => {
    alert('Button not implemented');
  };
}
