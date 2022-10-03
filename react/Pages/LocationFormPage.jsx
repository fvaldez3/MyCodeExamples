import React from 'react';
import LocationForm from '../../components/locations/LocationForm';

function LocationFormPage() {
    return (
        <React.Fragment>
            <div className="container">
                <div className="row d-flex justify-content-center">
                    <div className="col-6 m-4 shadow-lg p-3 mb-5 bg-white rounded">
                        <LocationForm></LocationForm>
                    </div>
                </div>
            </div>
        </React.Fragment>
    );
}

export default LocationFormPage;
