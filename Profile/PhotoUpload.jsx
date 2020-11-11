/* Photo upload section */
import React, { Component} from 'react';
import Cookies from 'js-cookie';
import {Button, Icon, Input, Image, Grid} from 'semantic-ui-react';

export default class PhotoUpload extends Component {

    constructor(props) {

        super(props);

        this.state = {

            file : null,



            previewing : false,
            profileImage : null

        }

        //this.filePicker = React.createRef()
        this.renderNoPhoto = this.renderNoPhoto.bind(this)
        this.HandleFileSelected = this.HandleFileSelected.bind(this)
        this.imageReader = this.imageReader.bind(this)
        this.renderPreviewingPhoto = this.renderPreviewingPhoto.bind(this)
        this.updatePhoto = this.updatePhoto.bind(this)

        this.testing = this.testing.bind(this)
       
        this.testing()
    };



    HandleFileSelected(event)
    {
        
        this.setState({
            file : event.target.files[0],
            previewing : true,
            imageId : ''
        })

    }
  


    updatePhoto()
    {
        const data = this.state.file
        //console.log(data)
        const formData = new FormData()
        formData.append('file', data, data.name)

        var cookies = Cookies.get('talentAuthToken');

        

        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/updateProfilePhoto',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                //'Content-Type': false 
            },
            type: "POST",
            processData: false,
            contentType: false,
            //contentType: "application/json",
            data: formData,
            //cache: false,
            //dataType: "file",
            success: function (res) {

                console.log(123)
                console.log(res.data)

                res ? console.log(res) : console.log('Nothing returned')

            }.bind(this),
            error: function(res)
            {
                console.log(res)
                console.log(88888888)
            }
        })
    }


    testing()
    {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({

            url: 'http://talentapi-profile-module1.azurewebsites.net/profile/profile/getImage',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            success: function (res) {

                console.log(res)
                console.log(1111111111)
                
                //res ? console.log('Talent Details: '+res.data) : console.log('Nothing returned')

                //console.log(this.state.profileData)
                

            }.bind(this),
            error: function(res)
            {
                console.log(res)
                console.log(222222222)
                //console.log(111)
            }
        })
    }



    imageReader(event)
    {

        const reader = new FileReader()
        reader.onload = () =>{

            if(reader.readyState == 2)
            {
                this.setState({
                    profileImage : reader.result,
                    previewing : true
                })
            }

        }

        reader.readAsDataURL(event.target.files[0])
        this.setState({
            file : event.target.files[0]
        })
        console.log(event.target.files[0])

    }
  

    renderNoPhoto()
    {
 
        console.log(this.state.file)
       

        return (

            this.props.imageId ? 

            <div className = 'ui sixteen wide column'>

                <label htmlFor = 'input'>
                    <Image 
                        size = 'small'
                        src = {this.props.imageId}
                        circular
                    />
                </label>

                <h6>Please refresh the page if first time upload profile photo and failed to display</h6>

                <input        
                        style = {{display : 'none'}}
                        id = 'input'
                        type = 'file'
                        accept = 'image/*'
                        onChange = {this.imageReader}
                    />  

                {/* <Button content = 'testing' onClick={this.testing}/> */}

            </div>

            :

            <div className = 'ui sixteen wide column'>

                <label htmlFor = 'input'>
                    <Icon 
                        name = 'camera retro'
                        size = 'huge'
                        circular
                    />
                </label>
                
                

                <input        
                    style = {{display : 'none'}}
                    //ref = {this.filePicker}
                    //hidden
                    id = 'input'
                    type = 'file'
                    accept = 'image/*'
                    //onChange = {this.HandleFileSelected}
                    onChange = {this.imageReader}
                />  
                
            </div>           
        )
    }
 

    renderPreviewingPhoto()
    {

        return(
            
            <Grid centered>
                <Grid.Row>
                    <Image 
                        circular
                        size = 'small'
                        src = {this.state.profileImage}
                        />
                    </Grid.Row>

                    <Grid.Row>
                    <Button
                        color = 'black'
                        content = 'Upload'
                        icon = 'upload'
                        onClick = {this.updatePhoto}
                        />
                </Grid.Row>
            </Grid>

        )

    }


    render() {

        return(

            this.state.previewing ? this.renderPreviewingPhoto() : this.renderNoPhoto()

        )       
    }
}
