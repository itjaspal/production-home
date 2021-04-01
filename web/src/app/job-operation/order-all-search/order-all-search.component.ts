import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrderReqSearchView } from '../../_model/job-operation-stock';
import { JobOperationStockService } from '../../_service/job-operation-stock.service';

@Component({
  selector: 'app-order-all-search',
  templateUrl: './order-all-search.component.html',
  styleUrls: ['./order-all-search.component.scss']
})
export class OrderAllSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: OrderReqSearchView,
    private _fb: FormBuilder,
    private _jobSvc: JobOperationStockService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : OrderReqSearchView  = new OrderReqSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    this.buildForm();
    
    
    this.model.user_id = this.data.user_id;
    
    
    
    console.log(this.model);
    //this.datas = await this._jobSvc.getOrderReqAll(this.model);
    console.log(this.datas);
   
        
  }

  private buildForm() {
    this.validationForm = this._fb.group({
    por_no: [null, []]
    });
  }

  async search() {
    this.model.por_no = this.model.por_no.toUpperCase();
    this.datas = await this._jobSvc.getOrderReqAll(this.model);

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
