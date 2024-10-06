import { Component } from '@angular/core';
import { EmployeeService } from 'src/app/service/employee.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  employees: any[] = [];
  employee = {
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    department: '',
    password: '',
    role: ''
  };
  isEditMode = false;

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.getEmployees();
  }

  getEmployees(): void {
    this.employeeService.getEmployees().subscribe(data => {
      this.employees = data;
    });
  }

  onSubmit(): void {
    if (this.isEditMode) {
      this.employeeService.updateEmployee(this.employee.id, this.employee).subscribe(() => {
        this.resetForm();
        this.getEmployees();
      });
    } else {
      this.employeeService.createEmployee(this.employee).subscribe(() => {
        this.resetForm();
        this.getEmployees();
      });
    }
  }

  onEdit(employee: any): void {
    this.isEditMode = true;
    this.employee = { ...employee }; // Copy the selected employee into the form
  }

  onDelete(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe(() => {
        this.getEmployees();
      });
    }
  }

  onCancel(): void {
    this.resetForm();
  }

  resetForm(): void {
    this.isEditMode = false;
    this.employee = {
      id: 0,
      firstName: '',
      lastName: '',
      email: '',
      department: '',
      password: '',
      role: ''
    };
  }
}