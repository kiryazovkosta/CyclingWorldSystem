// ------------------------------------------------------------------------------------------------
//  <copyright file="GetAllActivitiesQueryHandler.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Entities.Activities.Queries.GetAll;

using Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;
using Mapster;
using Models;

public class GetAllActivitiesQueryHandler
    : IQueryHandler<GetAllActivitiesQuery, IEnumerable<SimplyActivityResponse>>
{
    private readonly IActivityRepository activityRepository;

    public GetAllActivitiesQueryHandler(IActivityRepository activityRepository)
    {
        this.activityRepository = activityRepository ?? throw new ArgumentNullException(nameof(activityRepository));
    }

    public async Task<Result<IEnumerable<SimplyActivityResponse>>> Handle(
        GetAllActivitiesQuery request, 
        CancellationToken cancellationToken)
    {
        var activities = await this.activityRepository.GetAllAsync(cancellationToken);
        var response = activities.Adapt<IEnumerable<SimplyActivityResponse>>();
        return Result.Success(response);
    }
}