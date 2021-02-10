import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrderReqSearchView } from '../../_model/job-operation-stock';
import { ProductDefectService } from '../../_service/product-defect.service';

@Component({
  selector: 'app-product-defect-order-search',
  templateUrl: './product-defect-order-search.component.html',
  styleUrls: ['./product-defect-order-search.component.scss']
})
export class ProductDefectOrderSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: OrderReqSearchView,
    private _fb: FormBuilder,
    private _defectSvc: ProductDefectService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : OrderReqSearchView  = new OrderReqSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    this.buildForm();
    
    
    this.model.build_type = this.data.build_type;
    
    
    
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
        
    this.datas = await this._defectSvc.getOrderReq(this.model);

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
