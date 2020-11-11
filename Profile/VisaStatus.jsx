import React, { Component } from 'react'
import { Dropdown, Form } from 'semantic-ui-react';
//import { SingleInput } from '../Form/SingleInput.jsx';
import moment from 'moment';


export default class VisaStatus extends Component {

    constructor(props) {

        super(props);

        //console.log(moment('02/04/2018').format())



        this.state = {

            visaStatus : '',
            visaExpiryDate : ''

        }


        this.renderVisaType = this.renderVisaType.bind(this)
        this.renderVisaExpire = this.renderVisaExpire.bind(this)
        this.handleVisaTypeChange = this.handleVisaTypeChange.bind(this)
        this.handleVisaExpireChange = this.handleVisaExpireChange.bind(this)
    }



    handleVisaTypeChange(event, {value})
    {

        console.log(value)

        const data = value == 'Student Visa' || value == 'Work Visa' ?
        
        {
            visaStatus : value
        }

        :

        {
            visaStatus : value,
            visaExpiryDate : ''
        }

        this.setState({
            visaStatus : value,
            visaExpiryDate : this.props.visaExpiryDate
        })
        
        this.props.controlFunc(data)

    }


    handleVisaExpireChange(event, {value})
    {
        //const data = Object.assign({}, this.state.visaData)

        console.log(value)

        const data = {
            visaExpiryDate : value
        }

        this.setState({
            visaExpiryDate : value
        })

        console.log(this.state.visaExpiryDate)

        data.visaExpiryDate = value

        this.props.controlFunc(data)

    }


    renderVisaType()
    {
        
        const options = [

            {
                key: 'Citizen',
                text: 'Citizen',
                value: 'Citizen'
            },

            {
                key: 'Permanent Resident',
                text: 'Permanent Resident',
                value: 'Permanent Resident'
            },

            {
                key: 'Work Visa',
                text: 'Work Visa',
                value: 'Work Visa'
            },

            {
                key: 'Student Visa',
                text: 'Student Visa',
                value: 'Student Visa'
            }
        ]

        return(
      
                <Form.Field>
                    <label>Visa Type</label>
                        <Dropdown 
                            options = {options}
                            selection

                            placeholder = 'Select Visa Type'
                            value = {this.props.visaStatus}
                            onChange = {this.handleVisaTypeChange}
                            
                        />
                </Form.Field>

        )
   }
 
    renderVisaExpire()
    {
        //console.log(this.props.visaExpiryDate)
        const date = this.state.visaExpiryDate ? this.state.visaExpiryDate : this.props.visaExpiryDate
        //const date = this.props.visaExpiryDate ? this.props.visaExpiryDate : this.props.visaExpiryDate

        return(

            <Form.Field>
                
                {/* <Form.Input 
                    label = 'Visa Expired Date'
                    value = {date}
                    placeholder = 'DD/MM/YYYY'
                    onChange = {this.handleVisaExpireChange}
                /> */}

            <Form.Input 
                    label = 'Visa Expired Date'
                    value = {date}
                    placeholder = 'DD/MM/YYYY'
                    onChange = {this.handleVisaExpireChange}
                />

                
                
            </Form.Field>

        )   
    }
    

    render() {
        

        return(
            <div className = 'ui sixteen wide column'>
                
            <Form.Group>

                {this.renderVisaType()}
                {this.props.visaStatus == 'Student Visa' || this.props.visaStatus == 'Work Visa' ? this.renderVisaExpire() : null}

                
            </Form.Group>


            </div>
        )
      
    }
}