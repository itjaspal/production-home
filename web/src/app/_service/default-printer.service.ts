import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DisplayDefaultPrinterView, DefaultPrinterView } from '../_model/default-printer';

@Injectable({
  providedIn: 'root'
})
export class DefaultPrinterService {

  public user: any;

  constructor(
    private http: HttpClient,
   
  ) { 
   
  }

  public async getDefaultPrinter(_mcCode: number) {

    console.log('parameter _mcCode : '+ _mcCode);
    return await this.http.get<DisplayDefaultPrinterView>(environment.API_URL + 'defprinter/getInfo/' + _mcCode).toPromise(); 

  } 

  public async update(_model: DefaultPrinterView) {
    return await this.http.post<number>(environment.API_URL + 'defprinter/postSetDefault', _model).toPromise();
  } 
}
