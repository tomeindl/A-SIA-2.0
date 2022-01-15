import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InputService {
  public isClicked = false;
  public onMouseMoveSubject = new Subject<MouseEvent>();
  public onMouseUpSubject = new Subject<MouseEvent>();
  public onMouseDownSubject = new Subject<MouseEvent>();
  public onResize = new Subject<UIEvent>();
  public onScroll = new Subject<Event>();

  constructor() {
    document.addEventListener('mousemove', (e) => {
      this.onMouseMoveSubject.next(e);
    });
    document.addEventListener('mouseup', (e) => {
      this.onMouseUpSubject.next(e);
    });
    document.addEventListener('mousedown', (e) => {
      this.onMouseDownSubject.next(e);
    });
    window.addEventListener('resize', (e) => {
      this.onResize.next(e);
    });
    document.addEventListener('scroll', (e) => {
      this.onScroll.next(e);
    });
  }
}
