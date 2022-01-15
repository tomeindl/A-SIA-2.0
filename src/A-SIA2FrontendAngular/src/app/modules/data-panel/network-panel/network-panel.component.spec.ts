import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NetworkPanelComponent } from './network-panel.component';

describe('NetworkPanelComponent', () => {
  let component: NetworkPanelComponent;
  let fixture: ComponentFixture<NetworkPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NetworkPanelComponent]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NetworkPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
