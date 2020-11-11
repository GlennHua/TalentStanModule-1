import React, {Component} from 'react'
import Cookies from 'js-cookie'
import { default as Countries } from '../../../../util/jsonFiles/countries.json';
import { ChildSingleInput } from '../Form/SingleInput.jsx';
import { Button, List, Form, Dropdown } from 'semantic-ui-react';
import { countries } from '../Employer/common';

export class Address extends Component {

    constructor(props) {
        super(props)

        const details = this.props.details ? Object.assign({}, this.props.details) :
        {
            city: "",
            country: "",
            number: "",
            postCode: 0,
            street: "",
            suburb: ""
        }
       
        //console.log(details)
        
        this.state = {

            showEditSection : false,
            addressData : details,

            countryList : [],
            cityList : []

        }

        this.renderDisplay = this.renderDisplay.bind(this)
        this.renderEdit = this.renderEdit.bind(this)
        this.openEdit = this.openEdit.bind(this)
        this.closeEdit = this.closeEdit.bind(this)
        this.handleChange = this.handleChange.bind(this)
        this.saveChanges = this.saveChanges.bind(this)
        this.handleCountrySelection = this.handleCountrySelection.bind(this)
        this.handleCityChange = this.handleCityChange.bind(this)

    }


    componentDidMount()
    {

        // console.log(typeof(countries))
        // console.log(countries)
        // console.log(countries.China)

        const countryList = []
        Object.keys(countries).map(
            
            (country)=>{
                
                countryList.push({

                    "key": country,
                    "text": country,
                    "value": country

                })

            }
        )
        
        //console.log(countryList)
        this.setState({

            countryList : countryList

        })
    }

  

    openEdit()
    {
        const data = Object.assign({}, this.props.details)

        this.setState({

            showEditSection : true,
            addressData : data

        })
        console.log(this.state.addressData)
    }

    closeEdit()
    {
        this.setState({
            showEditSection : false
        })
    }

    handleChange(event)
    {
        const details = Object.assign({}, this.state.addressData)
        details[event.target.name] = event.target.value

        this.setState({

            addressData : details

        })
        console.log(details)
        console.log(this.state.addressData)
    }


    handleCountrySelection(e, {value})
    {
        const data = Object.assign({}, this.state.addressData)
        console.log(data)
        data.country = value
        this.setState({
            addressData : data
        })
        console.log(value)
        console.log(this.state.addressData)

        const cities = countries[value]
        console.log(cities)
         
        const cityOption = []

        Object.keys(cities).map(

                (city)=>{

                    cityOption.push({

                        "key" : cities[city],
                        "text" : cities[city],
                        "value" : cities[city]

                    })
                }
            )
        
            console.log(cityOption)
        
            this.setState({
                cityList : cityOption
        })

    }


    handleCityChange(e, {value})
    {
        const data = Object.assign({}, this.state.addressData)
        data.city = value

        this.setState({

            addressData : data

        })

        console.log(value)
        console.log(this.state.addressData)
    }

    saveChanges()
    {
        const newData = Object.assign({}, this.state.addressData)
        this.props.controlFunc(this.props.componentId, newData)
        this.closeEdit()
    }





    render() 
    {
        return(
            
            this.state.showEditSection ? this.renderEdit() : this.renderDisplay()
            //this.renderEdit()

        )
    }
 


    renderEdit()
    {

        
        //console.log('This is the testing From renderEdit Func: '+this.state.countryList[0].text)


        return(
                <div className = 'ui sixteen wide column'>
                
                
                    <Form.Group>

                        <Form.Field width = '3'>

                            <ChildSingleInput 
                                label = 'Number'
                                inputType = 'text'
                                name = 'number'
                                value = {this.state.addressData.number}
                                placeholder = 'Please Enter Number'
                                maxLength = {20}
                                controlFunc = {this.handleChange}
                                errorMessage = 'Please Enter Valid Info'
                            />

                        </Form.Field>


                        <Form.Field width = '10'>

                            <ChildSingleInput 
                                label = 'Street'
                                inputType = 'text'
                                name = 'street'
                                value = {this.state.addressData.street}
                                placeholder = 'Please Enter Street Name'
                                maxLength = {40}
                                controlFunc = {this.handleChange}
                                errorMessage = 'Please Enter Valid Info'
                            />

                        </Form.Field>


                        <Form.Field width = '3'>

                            <ChildSingleInput 
                                label = 'Suburb'
                                inputType = 'text'
                                name = 'suburb'
                                value = {this.state.addressData.suburb}
                                placeholder = 'Please Enter Suburb'
                                maxLength = {40}
                                controlFunc = {this.handleChange}
                                errorMessage = 'Please Enter Valid Info'
                            />   

                        </Form.Field>

                    </Form.Group>
                

                    <Form.Group>

                        <Form.Field width = '6'>
                            <label>Country</label>
                            <Dropdown 
                                placeholder = 'Select Your Country'
                                selection
                                text = {this.state.addressData.country}
                                options = {this.state.countryList}
                                onChange = {this.handleCountrySelection}
                                value = {this.state.addressData.country}
                            />
                        </Form.Field>


                        <Form.Field width = '7'>

                            <label>City</label>
                            <Dropdown 
                                placeholder = 'Select Your City'
                                selection
                                text = {this.state.addressData.city}
                                options = {this.state.cityList}
                                onChange = {this.handleCityChange}
                                value = {this.state.addressData.city}
                            />

                        </Form.Field>

                        <Form.Field width = '3'>

                            <ChildSingleInput 
                                label = 'Post Code'
                                inputType = 'number'
                                name = 'postCode'
                                value = {this.state.addressData.postCode}
                                placeholder = 'Please Enter Your Post Code'
                                maxLength = {25}
                                controlFunc = {this.handleChange}
                                errorMessage = 'Please Enter Valid Info'
                            />

                        </Form.Field>

                    </Form.Group>

                
                <Button 
                    content = 'Save'
                    onClick = {this.saveChanges}
                    color = 'black'
                    floated = 'left'
                />

                <Button 
                    content = 'Close'
                    onClick = {this.closeEdit}
                    floated = 'left'
                />

                </div>
            
        )
    }


    renderDisplay()
    {
        let address = this.props.details.number +', '+ this.props.details.street +', '+ this.props.details.suburb
        let city = this.props.details.city
        let country = this.props.details.country

        return(
            
            <div className = 'row'>
                <div className = 'ui sixteen wide column'>
                <React.Fragment>

                    <p>Address: {address}</p>
                    <p>City: {city}</p>
                    <p>Country: {country}</p>

                </React.Fragment>
                
                <Button 
                    content = 'Edit'
                    color = 'black'
                    onClick = {this.openEdit}
                    floated = 'right'
                />
               
                </div>
            </div>
        )

    }

  

}




















export class Nationality extends Component {

    constructor(props) {
        super(props)

        // const detail = this.props.nationalityData ? Object.assign({}, this.props.nationality) : {

        //     nationality : ""

        // }

        const detail = this.props.nationalityData

        
       
        //console.log(123)
        //console.log(detail)

        this.state = {

            nationality : detail,
            countryList : []

        }

        this.handleNationalityChange = this.handleNationalityChange.bind(this)

    }


    componentDidMount()
    {
        // console.log(typeof(countries))
        // console.log(countries)

        const nationalityList = []
        Object.keys(countries).map(

            (country)=>{

                nationalityList.push({

                    "key" : country,
                    "text" :  country,
                    "value" : country

                })

            }
        )

        //console.log(nationalityList)
        this.setState({

            countryList : nationalityList

        })
    }


    handleNationalityChange(e, {value})
    {
        //const newData = Object.assign({}, this.state.nationality)

        let newData = this.state.nationality

        // newData.nationality = value
        newData = value

        this.props.controlFunc(this.props.componentId, newData)

        this.setState({
            nationality : newData
        })

        // console.log(value)
        // console.log(this.state.nationality)
    }


  
    


    render() {

        // console.log(234)
        // console.log(this.state.nationality)

        return(

            <div className = 'ui sixteen wide column'>

                <Dropdown 

                    selection
                    options = {this.state.countryList}
                    placeholder = 'Please select your nationality'

                    onChange = {this.handleNationalityChange}
                    value = {this.props.nationalityData}

                />
                
            </div>

        )
        
    }
}