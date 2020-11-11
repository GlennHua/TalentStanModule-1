import React , { Component }from 'react'
import { Form, Checkbox, Button } from 'semantic-ui-react';
import { SingleInput } from '../Form/SingleInput.jsx';
import Cookies from 'js-cookie';

export default class TalentStatus extends Component {

    constructor(props) {

        super(props);
        
       this.handleEvent = this.handleEvent.bind(this)
       this.handleChange = this.handleChange.bind(this)
       this.getProps = this.getProps.bind(this)

    }
    

    
    
    getProps()
    {
        const status = Object.assign({}, this.props.status)
        //console.log(status)
        this.state = { status : status.status}
    }

    handleEvent(value)
    {
        const data = {

            status: value,
            availableDate: null
                      
        }

        this.props.controlFunc(this.props.componentId, data)

    }


    handleChange(event, {value})
    {
        console.log(value)
        
        this.setState({
            status : value
        })
        //const data = value

        this.handleEvent(value)
    }




    render() {

        this.getProps()
        console.log(this.state.status)

        return(
            <div className = 'ui sixteen wide column'>

                
            
            
                <Form.Field>
                    <label>Current Status</label>
                </Form.Field>


                <Form.Field>
                    <Checkbox 
                        radio
                        label = 'Actively looking for a job'
                        value = 'Actively looking for a job'
                        //name = 'checkBox'
                        checked = {this.state.status == 'Actively looking for a job'}
                        onChange = {this.handleChange}
                        

                    />
                </Form.Field>

 
                <Form.Field>
                    <Checkbox 
                        radio
                        label = 'Not looking for a job at the moment'
                        value = 'Not looking for a job at the moment'
                        //name = 'checkBox'
                        checked = {this.state.status == 'Not looking for a job at the moment'}
                        onChange = {this.handleChange}

                    />
                </Form.Field>


                <Form.Field>
                    <Checkbox 
                        radio
                        label = 'Currently employed but open to offers'
                        value = 'Currently employed but open to offers'
                        checked = {this.state.status == 'Currently employed but open to offers'}
                        onChange = {this.handleChange}

                    />
                </Form.Field>

                <Form.Field>
                    <Checkbox 
                        radio
                        label = 'Will be available on later date'
                        value = 'Will be available on later date'
                        checked = {this.state.status == 'Will be available on later date'}
                        onChange = {this.handleChange}

                    />
                </Form.Field>

                {/* <Button 
                    content = 'Testing'
                    onClick = {this.handleEvent}
                /> */}

                
            
            </div>
        )
        
    }
}