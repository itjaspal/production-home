import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';
import { DatePipe } from '@angular/common';
import { ProductSearchView } from '../../_model/job-inprocess';
import { ActivatedRoute } from '@angular/router';
import { data } from 'jquery';

@Component({
  selector: 'app-product-search',
  templateUrl: './product-search.component.html',
  styleUrls: ['./product-search.component.scss']
})
export class ProductSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductSearchView,
    // private _fb: FormBuilder,
    // private _msg: MessageService,
    // private _authSvc: AuthenticationService,
    // private _actRoute: ActivatedRoute,
    private _jobInprocessSvc: ScanInprocessService,
  ) { }

  //public validationForm: FormGroup;
  user:any = {};
  public model : ProductSearchView  = new ProductSearchView();
  
  datas: any = {};
  

  async ngOnInit() {
    var datePipe = new DatePipe("en-US");
    this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.model.pdjit_grp = this.data.pdjit_grp;
    this.model.wc_code = this.data.wc_code;
    
    //console.log(this.model);
    this.datas = await this._jobInprocessSvc.getproduct(this.model);

    console.log(this.datas);

  }
  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);

    
    console.log(addList);

    this.dialogRef.close(addList);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}
