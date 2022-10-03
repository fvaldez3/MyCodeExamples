import React from 'react';
import PropTypes from 'prop-types';
import swal from 'sweetalert2';
import { BsFillPencilFill, BsFillTrash2Fill, BsFillMapFill } from 'react-icons/bs';
import '../../components/locations/locationcard.css';

function LocationCard(props) {
    const location = props.locationData;

    const onDelete = (e) => {
        e.preventDefault();
        swal.fire({
            title: 'Are you sure?',
            text: 'This location will be deleted.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Confrim Deletion',
            confirmButtonColor: '#d63030',
        }).then((result) => {
            if (result.isConfirmed) {
                props.onDeleteRequest(location.id);
            }
        });
    };

    const onEdit = (e) => {
        e.preventDefault();
        props.onEditRequest(location);
    };

    const onShowMap = (e) => {
        e.preventDefault();
        props.showOnMap(location);
    };

    return (
        <React.Fragment>
            <div className={'card location-card bg-light border border-dark'}>
                <div className="card-body">
                    <div className="location-address-container">
                        <h5 className="card-title">{location.locationType.name}</h5>
                        <p className="location-address">{location.lineOne},</p>
                        {location.lineTwo ? (
                            <React.Fragment>
                                <p className="location-address">{location.lineTwo}</p>
                            </React.Fragment>
                        ) : (
                            ''
                        )}
                        <p className="location-address">
                            {location.city}, {location.state.name}
                        </p>
                        {location.zip ? (
                            <React.Fragment>
                                <p className="location-address">{location.zip}</p>
                            </React.Fragment>
                        ) : (
                            ''
                        )}
                    </div>
                    <div className="location-button-container">
                        <div className="row">
                            <div className="col-4">
                                <button onClick={onEdit} className="btn btn-info">
                                    {<BsFillPencilFill />}
                                </button>
                            </div>
                            <div className="col-4">
                                <button onClick={onDelete} className="btn btn-danger">
                                    {<BsFillTrash2Fill />}
                                </button>
                            </div>
                            <div className="col-4">
                                <button onClick={onShowMap} className="btn btn-success">
                                    {<BsFillMapFill />}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </React.Fragment>
    );
}

LocationCard.propTypes = {
    locationData: PropTypes.shape({
        id: PropTypes.number.isRequired,
        locationType: PropTypes.shape({
            id: PropTypes.number.isRequired,
            name: PropTypes.string.isRequired,
        }),
        lineOne: PropTypes.string.isRequired,
        lineTwo: PropTypes.string,
        city: PropTypes.string.isRequired,
        zip: PropTypes.string,
        state: PropTypes.shape({
            code: PropTypes.string.isRequired,
            id: PropTypes.number.isRequired,
            name: PropTypes.string.isRequired,
        }),
        latitude: PropTypes.number.isRequired,
        longitude: PropTypes.number.isRequired,
    }),
    onDeleteRequest: PropTypes.func.isRequired,
    onEditRequest: PropTypes.func.isRequired,
    showOnMap: PropTypes.func.isRequired,
};

export default React.memo(LocationCard);
