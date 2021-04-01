import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrderReqSearchView } from '../../_model/job-operation-stock';
import { ScanInprocessStockService } from '../../_service/scan-inprocess-stock.service';

@Component({
  selector: 'app-order-search',
  templateUrl: './order-search.component.html',
  styleUrls: ['./order-search.component.scss']
})
export class OrderSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: OrderReqSearchView,
    private _fb: FormBuilder,
    private _jobInprocessSvc: ScanInprocessStockService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : OrderReqSearchView  = new OrderReqSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    this.buildForm();
    
    
    this.model.wc_code = this.data.wc_code
    
    
    
    console.log(this.model);
    this.datas = await this._jobInprocessSvc.getOrderReq(this.model);
    console.log(this.datas);
   
        
  }

  private buildForm() {
    this.validationForm = this._fb.group({
    por_no: [null, []]
    });
  }

  async search() {
    this.model.por_no = this.model.por_no.toUpperCase();
    this.datas = await this._jobInprocessSvc.getOrderReq(this.model);

  }

  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);
    //console.log(addList);

    this.dialogRef.close(addList);
  }

  close()
  {
    this.dialogRef.close([]);
  }


}
