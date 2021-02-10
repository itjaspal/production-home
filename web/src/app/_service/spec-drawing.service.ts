import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SpecDrawingService {

  constructor(
    private http: HttpClient,
  ) { }
}
