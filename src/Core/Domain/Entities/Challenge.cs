namespace Domain.Entities;

using Common.Enumerations;
using Domain.Primitives;
using System;
using System.Collections.Generic;

public class Challenge : DeletableEntity
{
    public Challenge()
    {
        Users = new HashSet<UserChallenge>();
	}

    public string Title { get; set; } = null!;

	public string Description { get; set; } = null!;

	public DateTime BeginDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

	public ChallengeType ChallengeType { get; set; }

	public bool IsActive { get; set; }

	public ICollection<UserChallenge> Users { get; set; }
}