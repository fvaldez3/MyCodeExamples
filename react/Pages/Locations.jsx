import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import locationService from '../../services/locationService';
import debug from 'sabio-debug';
import LocationCard from '../../components/locations/LocationCard';
import toastr from 'toastr';
import GoogleMapWrapper from '../../components/locations/GoogleMapWrapper';
import Pagination from 'rc-pagination';
import '../filemanager/rcpagination.css';
import '../../assets/scss/icons.scss';
import '../../components/locations/locationpage.css';

const _logger = debug.extend('Locations');

function Locations() {
    const [locations, setLocations] = useState({ locationData: [], locationCards: [] });
    const [paginateData, setPaginateData] = useState({ totalCount: 0, currentPage: 1, pageIndex: 0, pageSize: 4 });
    const [isMapToggled, setIsMapToggle] = useState(false);
    const [marker, setMarker] = useState({ id: 0, lat: 0, lng: 0 });

    const navigate = useNavigate();

    useEffect(() => {
        locationService
            .getLocationsFromCurrentUser(paginateData.pageIndex, paginateData.pageSize)
            .then(onGetLocationsFromCurrentUserSuccess)
            .catch(onGetLocationsFromCurrentUserError);
    }, []);

    const onPageChange = (page) => {
        setPaginateData((prevState) => {
            const pd = { ...prevState };
            pd.currentPage = page;
            return pd;
        });
        locationService
            .getLocationsFromCurrentUser(page - 1, paginateData.pageSize)
            .then(onGetLocationsFromCurrentUserSuccess)
            .catch(onGetLocationsFromCurrentUserError);
    };

    const onGetLocationsFromCurrentUserSuccess = (response) => {
        setLocations((prevState) => {
            const locationData = { ...prevState };
            locationData.locationData = response.item.pagedItems;
            locationData.locationCards = response.item.pagedItems.map(mapLocationCard);
            return locationData;
        });
        setPaginateData((prevState) => {
            const pd = { ...prevState };
            pd.totalCount = response.item.totalCount;
            return pd;
        });
    };

    const onGetLocationsFromCurrentUserError = (err) => {
        _logger('onGetLocationsFromCurrentUserError', err);
    };

    const deleteLocation = (id) => {
        _logger('Will delete', id);
        const deleteHandler = getDeleteSuccessHandler(id);
        locationService.deleteLocation(id).then(deleteHandler).catch(onDeleteLocationError);
    };

    const getDeleteSuccessHandler = (idToBeRemoved) => {
        return () => {
            _logger('onDeleteLocationSuccess', idToBeRemoved);
            toastr.success('Deleted Successfully', 'Success');
            setLocations((prevState) => {
                const loc = { ...prevState };
                loc.locationData = [...loc.locationData];

                const index = loc.locationData.findIndex((location) => {
                    return location.id === idToBeRemoved;
                });

                if (index >= 0) {
                    loc.locationData.splice(index, 1);
                    loc.locationCards = loc.locationData.map(mapLocationCard);
                }

                return loc;
            });
        };
    };

    const onDeleteLocationError = (err) => {
        _logger('onDeleteLocationError', err);
        toastr.error('Deletion Failed', 'Error');
    };

    const editLocation = (location) => {
        const stateTransport = { type: 'LOCATION_EDIT', payload: location };
        navigate(`edit/${location.id}`, { state: stateTransport });
    };

    const showLocationOnMap = (location) => {
        setIsMapToggle(true);
        const markerObj = { lat: location.latitude, lng: location.longitude, id: location.id };
        setMarker(markerObj);
    };

    const goToNewForm = (e) => {
        navigate(e.currentTarget.dataset.page);
    };

    const toggleMap = () => {
        setIsMapToggle((prevState) => {
            return !prevState;
        });
    };

    const mapLocationCard = (location) => {
        return (
            <LocationCard
                locationData={location}
                key={`location_${location.id}`}
                onDeleteRequest={deleteLocation}
                onEditRequest={editLocation}
                showOnMap={showLocationOnMap}
            />
        );
    };

    return (
        <React.Fragment>
            <div className="container">
                <div className="row mt-3">
                    <div className="container">
                        <div className="row d-flex justify-content-center">
                            <div className="col-6 shadow-lg p-3 mb-5 bg-white rounded location-page-container me-4">
                                <div className="row">
                                    <div className="col">
                                        <h2>Your Locations</h2>
                                    </div>
                                    <div className="col-1 me-5">
                                        <button
                                            type="button"
                                            className="btn btn-primary btn-sm"
                                            onClick={goToNewForm}
                                            id="new"
                                            data-page="new">
                                            Add Location
                                        </button>
                                    </div>
                                    <div className="col-1 me-4">
                                        <button type="button" className="btn btn-secondary btn-sm" onClick={toggleMap}>
                                            {isMapToggled ? 'Hide Map' : 'Show Map'}
                                        </button>
                                    </div>
                                </div>
                                <div className="row">{locations.locationCards}</div>
                                <div className="col d-flex justify-content-start mt-3 mb-3">
                                    <Pagination
                                        className="filemanagerpagination"
                                        pageSize={paginateData.pageSize}
                                        total={paginateData.totalCount}
                                        current={paginateData.currentPage}
                                        onChange={onPageChange}></Pagination>
                                </div>
                            </div>
                            {isMapToggled && (
                                <React.Fragment>
                                    <div className="col-6 shadow-lg p-3 mb-5 bg-white rounded">
                                        <GoogleMapWrapper marker={marker}></GoogleMapWrapper>
                                    </div>
                                </React.Fragment>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </React.Fragment>
    );
}

export default Locations;
