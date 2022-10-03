import React, { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import debug from 'sabio-debug';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import locationFormSchema from '../../schemas/locationValidation';
import locationService from '../../services/locationService';
import toastr from 'toastr';
import './autocomplete.css';
import { useNavigate } from 'react-router-dom';
import LocationAutoComplete from './LocationAutoComplete';

const _logger = debug.extend('Locations').extend('LocationForm');

function LocationForm() {
    const [formState, setFormState] = useState({
        locationTypeId: 0,
        lineOne: '',
        lineTwo: '',
        city: '',
        zip: '',
        stateId: 0,
        latitude: 0,
        longitude: 0,
    });
    const [states, setStates] = useState([]);
    const [locationTypes, setLocationtypes] = useState([]);
    const { state } = useLocation();
    const navigate = useNavigate();

    useEffect(() => {
        if (state?.type === 'LOCATION_EDIT') {
            _logger('Incoming Data', state);
            setFormState(() => {
                const fs = { ...state.payload };
                if (fs.lineTwo === null) {
                    fs.lineTwo = '';
                }
                if (fs.zip === null) {
                    fs.zip = '';
                }
                fs.stateId = fs.state.id;
                fs.locationTypeId = fs.locationType.id;
                return fs;
            });
        } else {
            _logger('No Data was passed');
        }

        locationService.getStates().then(onGetStatesSuccess).catch(onGetStatesError);
        locationService
            .getLocationTypes(['LocationTypes'])
            .then(onGetLocationTypeSuccess)
            .catch(onGetLocationTypeError);
    }, []);

    const submitForm = (values) => {
        if (values.lineTwo === '') {
            values.lineTwo = null;
        }
        if (values.zip === '') {
            values.zip = null;
        }
        if (state) {
            locationService.updateLocation(values).then(onUpdateLocationSuccess).catch(onUpdateLocationError);
        } else {
            locationService.addLocation(values).then(onAddLocationSuccess).catch(onAddLocationError);
        }
    };

    const mapLookUpData = (lookUpObj) => {
        return (
            <option value={lookUpObj.id} key={`state_${lookUpObj.id}`}>
                {lookUpObj.name}
            </option>
        );
    };

    const onAddLocationSuccess = (response) => {
        _logger('onAddLocationSuccess', response);
        toastr.success('Location was added successfully', 'Success!');
        navigate('/locations');
    };

    const onAddLocationError = (err) => {
        _logger('onAddLocationError', err);
        toastr.error('Failed to add Location', 'Error..');
    };

    const onUpdateLocationSuccess = (response) => {
        _logger('onUpdateLocationSuccess', response);
        toastr.success('Location was updated successfully', 'Success!');
        navigate('/locations');
    };

    const onUpdateLocationError = (err) => {
        _logger('onUpdateLocationError', err);
        toastr.error('Failed to update Location', 'Error..');
    };

    const onGetStatesSuccess = (response) => {
        setStates(response.items);
    };

    const onGetStatesError = (err) => {
        _logger('onGetStatesError', err);
        toastr.error('Failed to retrieve States', 'Error');
    };

    const onGetLocationTypeSuccess = (response) => {
        setLocationtypes(response.item.locationTypes);
    };

    const onGetLocationTypeError = (err) => {
        _logger('onGetLocationTypeError', err);
        toastr.error('Failed to retrieve Location Types', 'Error');
    };

    const renderButtons = () => {
        return (
            <React.Fragment>
                <div className="row">
                    <div className="col  d-flex justify-content-center">
                        <button type="submit" className={state ? 'btn btn-info mt-3' : 'btn btn-primary mt-3'}>
                            {state ? 'Update' : 'Add'}
                        </button>
                    </div>
                </div>
            </React.Fragment>
        );
    };

    const handleChange = (address) => {
        if (!address.incomplete) {
            const stateIndex = states.findIndex((state) => {
                return state.name === address.state;
            });
            const state = states[stateIndex];
            setFormState((prevState) => {
                const fs = { ...prevState };
                fs.latitude = address.latitude;
                fs.longitude = address.longitude;
                fs.lineOne = address.lineOne;
                fs.city = address.city;
                fs.zip = address.zip;
                fs.stateId = state.id;
                fs.locationTypeId = prevState.locationTypeId;
                fs.id = prevState.id;
                return fs;
            });
        } else {
            _logger('An address was not selected');
        }
    };
    return (
        <React.Fragment>
            <div className="container">
                <h3>Location Form</h3>
                <Formik
                    enableReinitialize={true}
                    initialValues={formState}
                    onSubmit={submitForm}
                    validationSchema={locationFormSchema}>
                    {({ values }) => (
                        <Form>
                            <div>
                                <LocationAutoComplete handleChange={handleChange} />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="lineOne">Line One</label>
                                <Field
                                    type="text"
                                    name="lineOne"
                                    disabled
                                    value={values.lineOne}
                                    className="form-control"
                                />
                                <ErrorMessage name="lineOne" component="div" className="text-danger" />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="lineTwo">{`Line Two (Optional)`}</label>
                                <Field type="text" name="lineTwo" className="form-control" />
                                <ErrorMessage name="lineTwo" component="div" className="text-danger" />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="city">City</label>
                                <Field type="text" name="city" className="form-control" />
                                <ErrorMessage name="city" component="div" className="text-danger" />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="zip">{`Zip Code (Optional)`}</label>
                                <Field type="text" name="zip" className="form-control" />
                                <ErrorMessage name="zip" component="div" className="text-danger" />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="stateId">State</label>
                                <Field component="select" name="stateId" className="form-control">
                                    <option value=""> Please Select a State</option>
                                    {states.map(mapLookUpData)}
                                </Field>
                                <ErrorMessage name="stateId" component="div" className="text-danger" />
                            </div>
                            <div className="form-group mt-2">
                                <label htmlFor="locationTypeId">Location Type</label>
                                <Field component="select" name="locationTypeId" className="form-control">
                                    <option value=""> Please Select Location Type</option>
                                    {locationTypes.map(mapLookUpData)}
                                </Field>
                                <ErrorMessage name="locationTypeId" component="div" className="text-danger" />
                            </div>
                            {renderButtons()}
                        </Form>
                    )}
                </Formik>
            </div>
        </React.Fragment>
    );
}
export default React.memo(LocationForm);
